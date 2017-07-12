#import <MobileCenterDistribute/MobileCenterDistribute.h>

typedef bool (__cdecl *ReleaseAvailableWithDetailsFunction)(MSReleaseDetails*);

extern "C" void mobile_center_unity_distribute_set_delegate();
extern "C" void mobile_center_unity_distribute_delegate_provide_release_available_impl(ReleaseAvailableWithDetailsFunction* functionPtr);


@interface UnityDistributeDelegate : NSObject<MSDistributeDelegate>
- (BOOL)distribute:(MSDistribute *)distribute releaseAvailableWithDetails:(MSReleaseDetails *)details;
@end
