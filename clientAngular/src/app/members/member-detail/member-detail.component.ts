import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  NgxGalleryAnimation,
  NgxGalleryImage,
  NgxGalleryOptions,
} from '@kolkov/ngx-gallery';
import { ReplaySubject } from 'rxjs';
import { Member } from 'src/app/_models/model';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
  memberSource: ReplaySubject<Member> = new ReplaySubject<Member>(1);
  currentMember$ = this.memberSource.asObservable();
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];

  constructor(
    private memberService: MembersService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadMember();

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      },
    ];

    this.getImages();
  }

  getImages() {
    const imageUrls: any[] = [];
    this.currentMember$.subscribe((member) => {
      for (const photo of member.photos) {
        imageUrls.push({
          small: photo?.url,
          medium: photo?.url,
          big: photo?.url,
        });
      }
      this.galleryImages = imageUrls;
    });
  }

  loadMember() {
    this.memberService
      .getMember(this.route.snapshot.paramMap.get('username') || '')
      .subscribe((member) => {
        this.memberSource.next(member);
      });
  }
}
